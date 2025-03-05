import re
import matplotlib.pyplot as plt
import argparse

def parse_simulation_output(file_path):
    cow_counts = []
    tiger_counts = []
    tree_counts = []
    
    with open(file_path, 'r') as file:
        for line in file:
            cow_match = re.search(r'Cow:\s*(\d+)', line)
            tiger_match = re.search(r'Tiger:\s*(\d+)', line)
            tree_match = re.search(r'Tree:\s*(\d+)', line)

            if cow_match:
                cow_counts.append(int(cow_match.group(1)))
            if tiger_match:
                tiger_counts.append(int(tiger_match.group(1)))
            if tree_match:
                tree_counts.append(int(tree_match.group(1)))
    
    return cow_counts, tiger_counts, tree_counts

def plot_predator_prey(cows, tigers):
    plt.figure(figsize=(10, 5))
    plt.plot(range(len(cows)), cows, label='Cows (Prey)', color='blue')
    plt.plot(range(len(tigers)), tigers, label='Tigers (Predator)', color='red')
    plt.xlabel('Time Steps')
    plt.ylabel('Population')
    plt.title('Predator-Prey Population Dynamics')
    plt.legend()
    plt.grid()
    plt.show()

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Parse simulation output and plot predator-prey dynamics.")
    parser.add_argument("filepath", type=str, help="Path to the simulation output file")
    args = parser.parse_args()
    
    cow_population, tiger_population, _ = parse_simulation_output(args.filepath)
    plot_predator_prey(cow_population, tiger_population)
