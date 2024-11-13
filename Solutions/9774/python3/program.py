# Get number as string
number = input()

# Print digits with correct format
for char_digit in number: 
    print(f"{char_digit}:", end=" ")
    
    for _ in range(int(char_digit)):
        print(char_digit, end="")
    
    print("", end="\n")