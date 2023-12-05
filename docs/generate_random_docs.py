import json
import random
import string
import gzip
import itertools

def generate_random_string(length):
    letters = string.ascii_letters
    return ''.join(random.choice(letters) for _ in range(length))

def generate_random_number():
    return random.randint(1, 100)

def generate_random_bool():
    return random.choice([True, False])

def generate_random_json_data(num_entries):
    data = []
    for i in range(num_entries):
        entry = {
            "id": str(i),
            "name": generate_random_string(8),
            "age": generate_random_number(),
            "is_student": generate_random_bool(),
            "fat_payload" : generate_random_string(2000)
        }
        data.append(entry)
    return data

def write_entries(num_entries: int, fname: str):
    random_json_data = generate_random_json_data(num_entries)

    # Write JSON data to a gzipped file
    with gzip.open(fname, "wt", encoding="utf-8") as gzip_file:
        for entry in random_json_data:
            json_entry = json.dumps(entry)
            gzip_file.write(json_entry + '\n')


write_entries(10000, "1.json.gz")