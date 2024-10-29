import math

# Define area calculation functions
calc_area_functions = {
    'square': lambda x: x ** 2,
    'circle': lambda r: math.pi * r ** 2,
    'rectangle': lambda x, y: x * y,
    'triangle': lambda base, height: 0.5 * base * height,
}


def get_func(shapes):
    """Retrieve area calculation functions for the given shapes."""
    return [calc_area_functions[shape] for shape in shapes]
