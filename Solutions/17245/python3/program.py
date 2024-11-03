line1 = input()
line2 = input()
line3 = input()
len_lines = len(line1)

Taxlangs = {}
for i in range(0, len_lines, 5):
    new_code = line1[i : i + 5]
    count_star = new_code.count("*")
    Taxlangs[i] = [count_star]
for i in range(0, len_lines, 5):
    new_code = line2[i : i + 5]
    count_star = new_code.count("*")
    Taxlangs[i] += [count_star]
for i in range(0, len_lines, 5):
    new_code = line3[i : i + 5]
    count_star = new_code.count("*")
    Taxlangs[i] += [count_star]

ans_code = ""

for value in Taxlangs.values():
    if value == [5, 1, 1]:
        ans_code += "T"
    elif value == [1, 3, 2]:
        ans_code += "A"
    elif value == [2, 1, 2]:
        ans_code += "X"
    elif value == [4, 3, 2]:
        ans_code += "M"
    else:
        ans_code += "N"

print(ans_code)