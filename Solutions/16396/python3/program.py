class Foo:
    def __init__(self):
        self._x = 0  # Default value for x

    @property
    def x(self):
        return self._x

    @x.setter
    def x(self, value):
        if value < 0:
            self._x = -1
        else:
            self._x = value % 100


p = Foo()
print(p.x)

p.x = 125
print(p.x)

p.x = 15874
print(p.x)

p.x = 8
print(p.x)

p.x = 13
print(p.x)

p.x = -125
print(p.x)

p.x = 1234
print(p.x)
