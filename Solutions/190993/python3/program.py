def get_inputs(num_of_lines: int) -> dict[str, str]:
    """Reads input lines and creates a dictionary mapping binary to label."""
    return dict(input().split() for _ in range(num_of_lines))


def main():
    """Main function to process the dataset and handle queries."""
    n, q, _ = map(int, input().split())  # Simplified input parsing
    dataset = get_inputs(n)  # Populate the dataset

    for _ in range(q):
        query = input()
        print(dataset.get(query, "Unknown"))  # Use `get` for safe lookups


if __name__ == "__main__":
    main()
