import subprocess
import argparse
import datetime
import matplotlib.pyplot as plt

def get_git_commit_data():
    """
    Returns a list of commit data, including timestamps, hashes, messages, and formatted date-time.
    """
    try:
        result = subprocess.run(
            ["git", "log", "--pretty=format:%ct %h %s"],  # Get timestamp, hash, and message
            capture_output=True,
            text=True,
            check=True
        )
        commits = []
        for line in result.stdout.strip().split("\n"):
            parts = line.split(" ", 2)
            if len(parts) == 3:
                timestamp, commit_hash, message = parts
                timestamp = int(timestamp)
                commit_date = datetime.datetime.fromtimestamp(timestamp).strftime('%Y-%m-%d %H:%M:%S')
                commits.append((timestamp, commit_hash, message, commit_date))

        return commits
    except subprocess.CalledProcessError as e:
        print(f"Error running git command: {e}")
        return []

def compute_commit_durations(commits, max_interval, verbose=False):
    """
    Computes time differences (in minutes) between consecutive commits.
    - Prints **all intervals** in verbose mode.
    - Returns only intervals **less than max_interval** for histogram plotting.
    """
    durations = []
    filtered_durations = []

    for i in range(len(commits) - 1):
        ts_current, hash_current, msg_current, date_current = commits[i]
        ts_previous, hash_previous, msg_previous, date_previous = commits[i + 1]

        diff_seconds = ts_current - ts_previous  # Git log is in descending order
        diff_minutes = diff_seconds / 60  # Convert to minutes
        
        durations.append(diff_minutes)  # Store all intervals

        if verbose:
            print(f"{date_previous} | {hash_previous} -> {hash_current} | {diff_minutes:.2f} min")
            print(f"  {msg_previous} -> {msg_current}\n")

        if diff_minutes <= max_interval:  # Only keep shorter intervals for histogram
            filtered_durations.append(diff_minutes)

    return durations, filtered_durations

def plot_histogram(durations, bins):
    """Plots a histogram of commit durations with a user-specified number of bins."""
    plt.figure(figsize=(10, 6))
    plt.hist(durations, bins=bins, edgecolor='black', alpha=0.7)
    plt.xlabel("Time Between Commits (minutes)")
    plt.ylabel("Frequency")
    plt.title(f"Histogram of Time Intervals Between Git Commits (Bins: {bins})")
    plt.grid(axis="y", linestyle="--", alpha=0.7)
    plt.show()

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Plot a histogram of Git commit intervals.")
    parser.add_argument("bins", type=int, help="Number of bins for the histogram.")
    parser.add_argument("max_interval", type=int, help="Maximum interval (in minutes) to include in the histogram.")
    parser.add_argument("--verbose", action="store_true", help="Print commit history with all durations.")
    args = parser.parse_args()

    commits = get_git_commit_data()
    
    if commits:
        all_durations, filtered_durations = compute_commit_durations(commits, args.max_interval, args.verbose)
        
        if filtered_durations:
            plot_histogram(filtered_durations, args.bins)
        else:
            print(f"No commit intervals below {args.max_interval} minutes found for the histogram.")
    else:
        print("No commit data found.")
