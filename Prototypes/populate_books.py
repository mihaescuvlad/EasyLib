import json
import requests
import pyodbc
import uuid

def get_connection_string():
    with open('../Application/appsettings.Development.json') as f:
        data = json.load(f)
    server = data["ConnectionStrings"]["AZURE_CONNECTION_STRING"].split(';')[0].split('=')[1]
    database = data["ConnectionStrings"]["AZURE_CONNECTION_STRING"].split(';')[1].split('=')[1]

    connection_string = f'DRIVER={{ODBC Driver 17 for SQL Server}};SERVER={server};DATABASE={database}'

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

            cursor.execute("SELECT Id FROM LibraryLocations")
            locations = cursor.fetchall()
            for location_id in locations:
                cursor.execute("INSERT INTO BookStocks (BookIsbn, LibraryId, Stock) VALUES (?, ?, ?)", (isbn, location_id[0], 2))
                conn.commit()
        else:
            print(f"Book with ISBN '{isbn}' already exists. Skipping insertion.")

    conn.close()

def check_library_locations():
    connection_string = get_connection_string()
    conn = pyodbc.connect(connection_string)
    cursor = conn.cursor()

    cursor.execute("SELECT COUNT(*) FROM LibraryLocations")
    count = cursor.fetchone()[0]

    conn.close()

    return count == 0

def create_library_locations():
    if not check_library_locations():
        print("Library locations already exist. Skipping creation.")
        return

    connection_string = get_connection_string()
    conn = pyodbc.connect(connection_string)
    cursor = conn.cursor()

    open_time = "08:00:00"
    close_time = "20:00:00"

    libraries = [
        {"address1": "Dolj, Craiova", "address2": "Aleea Pietrelor Metin"},
        {"address1": "Dolj, Craiova", "address2": "Strada Sabiilor +9"},
        {"address1": "Braila, Braila", "address2": "Taramul Sfant"},
        {"address1": "Ierusalim", "address2": None},
    ]

    address_ids = []
    for library in libraries:
        address_id = str(uuid.uuid4())
        cursor.execute("INSERT INTO Addresses (Id, Address1, Address2) VALUES (?, ?, ?)",
                       (address_id, library["address1"], library["address2"]))
        conn.commit()
        address_ids.append(address_id)

    for address_id in address_ids:
        cursor.execute("INSERT INTO LibraryLocations (Id, AddressId, OpenTime, CloseTime) VALUES (?, ?, ?, ?)",
                       (str(uuid.uuid4()), address_id, open_time, close_time))
        conn.commit()

    conn.close()

def main():
    genres = ["fiction", "fantasy", "computers", "history", "science"]

    create_library_locations()
    print("Library locations created successfully.")

    for genre in genres:
        books = fetch_book_data(genre)

        insert_books(books)

        print(f"Books and authors for genre '{genre}' added to Azure SQL Server.")

if __name__ == "__main__":
    main()
