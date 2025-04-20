import os

def prefix_files_with_anim(directory):
    for filename in os.listdir(directory):
        if filename.startswith("anim_") or not os.path.isfile(os.path.join(directory, filename)):
            continue  # Skip already-prefixed or non-files
        old_path = os.path.join(directory, filename)
        new_filename = f"anim_{filename}"
        new_path = os.path.join(directory, new_filename)
        os.rename(old_path, new_path)
        print(f"Renamed: {filename} -> {new_filename}")

if __name__ == "__main__":
    current_dir = os.path.dirname(os.path.abspath(__file__))
    prefix_files_with_anim(current_dir)
