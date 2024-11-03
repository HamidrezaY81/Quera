from math import floor, log, sqrt

def get_prime_factors(n):
    prime_factors = []
    while n % 2 == 0:
        prime_factors.append(2)
        n = n / 2
         
    for i in range(3,int(sqrt(n))+1,2):
         
        while n % i== 0:
            prime_factors.append(i)
            n = n / i
             

    if n > 2:
        prime_factors.append(n)
        
    return prime_factors

t = int(input())

results = ''

for _ in range(t):
    n, m = map(int, input().split())

    prime_factors_dup = get_prime_factors(m)
    prime_factors_uniq = set(prime_factors_dup)
    a_i = list(map(lambda x: prime_factors_dup.count(x), prime_factors_uniq))
    count = 0

    for index, p in enumerate(prime_factors_uniq):
        max_loop = floor(log(n, p))
        min_loop = 1
        sum_zigma = 0
        for i in range(min_loop, max_loop + 1):
            sum_zigma += floor(n / (p ** i))
        if sum_zigma >= a_i[index]:
            count += 1
    
    if count == len(a_i):
        results += f"{m} divides {n}!\n"
    else: 
        results += f"{m} does not divide {n}!\n"
 

print(results.strip('\n'))