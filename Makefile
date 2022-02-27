CC=g++
LD=g++
IN=iscc
AS=gcc

CCFLAGS=-Iinclude -Ilib/rutils/include
CCFLAGS+=-pedantic -Wall -Wextra -Wcast-align -Wcast-qual -Wctor-dtor-privacy \
-Wdisabled-optimization -Wformat=2 -Winit-self -Wlogical-op -Wmissing-declarations \
-Wmissing-include-dirs -Wnoexcept -Wold-style-cast -Woverloaded-virtual \
-Wshadow -Wsign-conversion -Wsign-promo -Wstrict-null-sentinel -Wstrict-overflow=5 \
-Wundef -Wno-unused -Werror -Wredundant-decls -Wswitch-default
LDFLAGS=

SOURCE=$(wildcard src/*.cpp)
OBJECT=$(patsubst src/%.cpp, bin/%.o, $(SOURCE))

OUT=buckle.exe

all: $(OUT)

bin/%.o: src/%.cpp
	$(CC) $(CCFLAGS) -c $^ -o $@

$(OUT): $(OBJECT)
	$(LD) $(LDFLAGS) $^ -o $@

setup:
	mkdir -p bin

clean:
	rm -f bin/*
	rm $(OUT)