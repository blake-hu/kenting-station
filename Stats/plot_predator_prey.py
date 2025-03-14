import re
import matplotlib.pyplot as plt
import argparse
import time

def parse_simulation_output(file_path):
    cow_counts = []
    tiger_counts = []
    tree_counts = []
    grass_counts = []
    
    with open(file_path, 'r') as file:
        for line in file:
            cow_match = re.search(r'Cow:\s*(\d+)', line)
            if cow_match:
                cow_counts.append(int(cow_match.group(1)))
            
            tiger_match = re.search(r'Tiger:\s*(\d+)', line)
            if tiger_match:
                tiger_counts.append(int(tiger_match.group(1)))
            
            tree_match = re.search(r'Tree:\s*(\d+)', line)
            if tree_match:
                tree_counts.append(int(tree_match.group(1)))
            
            grass_match = re.search(r'Grass:\s*(\d+)', line)
            if grass_match:
                grass_counts.append(int(grass_match.group(1)))
    
    return cow_counts, tiger_counts, tree_counts, grass_counts

def plot_predator_prey(cows, tigers, grass):
    plt.clf()  # Clear the current figure
    plt.plot(range(len(cows)), cows, label='Cows (Prey)', color='blue')
    plt.plot(range(len(tigers)), tigers, label='Tigers (Predator)', color='red')
    plt.plot(range(len(grass)), grass, label='Grass (Food Source)', color='green')
    plt.xlabel('Time Steps')
    plt.ylabel('Population')
    plt.title('Predator-Prey-Resource Population Dynamics')
    plt.legend()
    plt.grid()
    plt.draw()
    plt.savefig('./plot.png', format='png')
    plt.pause(3)  # Pause to update the plot every 3 seconds

def continuously_plot(filepath):
    plt.ion()  # Enable interactive mode
    while True:
        cow_population, tiger_population, _, grass_population = parse_simulation_output(filepath)
        plot_predator_prey(cow_population, tiger_population, grass_population)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Parse simulation output and plot predator-prey-resource dynamics.")
    parser.add_argument("filepath", type=str, help="Path to the simulation output file")
    args = parser.parse_args()
    
    continuously_plot(args.filepath)
