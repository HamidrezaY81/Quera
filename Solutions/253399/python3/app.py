# Take inputs in correct format
n = int(input())
s, x = input().split()

# Create player-hands dict
players = ['1', '2', '3']
hands = ['L', 'R']
players_dict = {}
for player in players:
    players_dict[player] = {}
    for hand in hands:
        players_dict[player][hand] = None

# Initilize gol 
players_dict[s][x] = "Gol"

# Move gol base on moves
for _ in range(n):
    move_info = input().split()
    move_type = int(move_info[0])

    if move_type == 1: 
        type, player = move_info
        temp = players_dict[player]['R']
        players_dict[player]['R'] = players_dict[player]['L']
        players_dict[player]['L'] = temp
    elif move_type == 2:
        type, player, x, y = move_info
        next_player = str(int(player) + 1)
        temp = players_dict[next_player][str(y)]
        players_dict[next_player][str(y)] = players_dict[player][str(x)]
        players_dict[player][str(x)] = temp

# Find gol and print when found
for player in players_dict:
    is_found = False
    player_hands = players_dict[player]
    for hand in player_hands:
        if player_hands[hand] == 'Gol':
            print(f"{player} {hand}")
            is_found = True
            break
    if is_found:
        break