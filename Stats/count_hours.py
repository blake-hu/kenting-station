import subprocess
import argparse

def get_git_commit_timestamps():
    """Returns a list of timestamps for all git commits in the current repository."""
    try:
        result = subprocess.run(
            ["git", "log", "--pretty=%ct"],
            capture_output=True,
            text=True,
            check=True
        )
        timestamps = [int(ts) for ts in result.stdout.strip().split("\n") if ts]
        return timestamps
    except subprocess.CalledProcessError as e:
        print(f"Error running git command: {e}")
        return []

def calculate_close_commit_time(timestamps, threshold_minutes):
    """Calculates the total time (in minutes) between commits that are less than the given threshold apart."""
    total_minutes = 0

    for i in range(len(timestamps) - 1):
        diff_seconds = timestamps[i] - timestamps[i + 1]  # Git log is in descending order
        diff_minutes = diff_seconds / 60

        if diff_minutes <= threshold_minutes:
            total_minutes += diff_minutes

    return round(total_minutes)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Calculate accumulated minutes for commits within a time threshold.")
    parser.add_argument("threshold", type=int, help="The number of minutes between commits to count towards the total.")
    args = parser.parse_args()

    timestamps = get_git_commit_timestamps()
    if timestamps:
        total_time = calculate_close_commit_time(timestamps, args.threshold)
        print(f"Total accumulated minutes for commits less than {args.threshold} minutes apart: {total_time} minutes")
    else:
        print("No commit timestamps found.")
