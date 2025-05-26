import json
import pyodbc
import os
from dotenv import load_dotenv

load_dotenv()
conn_str = os.getenv("SQLSERVER_CONNECTION_STRING")


conn = pyodbc.connect(conn_str)
cursor = conn.cursor()

with open("dentist_everything.json", encoding="utf-8") as f:
    data = json.load(f)


for item in data:
    place_id = item["place_id"]
    name = item["name"]
    address = item["address"]
    lat = item["lat"]
    lng = item["lng"]
    rating = item["rating"]
    website = item["website"]
    
    if item["website"] == "N/A":
        website = None

    cursor.execute("INSERT INTO hospital_data (place_id, name, address, lat, lng, rating, website) VALUES (?, ?, ?, ?, ?, ?, ?)",
                   place_id, name, address, lat, lng, rating, website)

conn.commit()
conn.close()
