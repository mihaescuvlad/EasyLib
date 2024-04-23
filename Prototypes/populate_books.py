import json
import requests
import pyodbc
import uuid

def get_connection_string():
    with open('../Application/appsettings.Development.json') as f:
        data = json.load(f)
    server = data["ConnectionStrings"]["AZURE_CONNECTION_STRING"].split(';')[0].split('=')[1]
    database = data["ConnectionStrings"]["AZURE_CONNECTION_STRING"].split(';')[1].split('=')[1]
    username = data["ConnectionStrings"]["AZURE_CONNECTION_STRING"].split(';')[3].split('=')[1]
    password = data["ConnectionStrings"]["AZURE_CONNECTION_STRING"].split(';')[4].split('=')[1]

    connection_string = f'DRIVER={{ODBC Driver 17 for SQL Server}};SERVER={server};DATABASE={database};UID={username};PWD={password}'
    return connection_string

def fetch_book_data(genre):
    url = f"https://www.googleapis.com/books/v1/volumes?q=subject:{genre}&maxResults=40"
    response = requests.get(url)
    data = response.json()
    books = data.get("items", [])
    return books

def insert_books(book_data):
    connection_string = get_connection_string()
    conn = pyodbc.connect(connection_string)
    cursor = conn.cursor()

    for book in book_data:
        if "volumeInfo" not in book or "industryIdentifiers" not in book["volumeInfo"] or \
                not book["volumeInfo"]["industryIdentifiers"]:
            print("Skipping book due to missing or invalid data.")
            continue

        isbn = book["volumeInfo"]["industryIdentifiers"][0]["identifier"]
        title = book["volumeInfo"]["title"]
        description = book["volumeInfo"].get("description", "")
        thumbnail = book["volumeInfo"].get("imageLinks", {}).get("thumbnail", "")

        cursor.execute("SELECT COUNT(*) FROM Books WHERE Isbn = ?", (isbn,))
        count = cursor.fetchone()[0]

        if count == 0:
            cursor.execute("INSERT INTO Books (Isbn, Title, Description, Thumbnail) VALUES (?, ?, ?, ?)",
                           (isbn, title, description, thumbnail))
            conn.commit()

            authors = book["volumeInfo"].get("authors", [])
            for author_name in authors:
                author_id = str(uuid.uuid4())

                cursor.execute("IF NOT EXISTS (SELECT 1 FROM Authors WHERE AuthorName = ?) BEGIN INSERT INTO Authors (Id, AuthorName) VALUES (?, ?) END",
                               (author_name, author_id, author_name))
                conn.commit()

                cursor.execute("INSERT INTO BookAuthors (BookIsbn, AuthorId) VALUES (?, ?)",
                               (isbn, author_id))
                conn.commit()
        else:
            print(f"Book with ISBN '{isbn}' already exists. Skipping insertion.")

    conn.close()

def main():
    genres = ["fiction", "fantasy", "computers", "history", "science"]

    for genre in genres:
        books = fetch_book_data(genre)

        insert_books(books)

        print(f"Books and authors for genre '{genre}' added to Azure SQL Server.")

if __name__ == "__main__":
    main()
