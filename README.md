# BELTE Language & Buckle Compiler

This document covers what BELTE and Buckle are, their design principles, and how they are able to do optimizations to cut out physical concerns.
This document **does not** cover the syntax and descriptions of BELTE and its libraries.

> [Documentation for BELTE](docs/Index.md)
> [Documentation for Buckle](docs/Buckle.md)

For sake of looking back this project started in December of 2021.

## Introduction

BELTE is a statically and strongly typed language most similar to C# (BELTE is object oriented), compiled using the Buckle compiler.

This language and compiler is being developed to solve the biggest issue with all modern general purpose programming languages.
This is the distinction of **Logical vs Physical**. Low-level implementation details should not be a concern to the developer\*.

To better understand this distinction take the code example (in C++):

```cpp
struct MyStruct {
    int val1;
    int val2;
};

class MyClass {
public:
    int val1;
    int val2;
};
```

The only reason to use a structure over a class is more efficiency. In this example in particular the functionality of each is identical.
The developer should not need to engross themselves in these details while coding. The compiler should choose depending on which is better.

To get an idea of how the compiler could achieve this, lets look at another example:

```cpp
class MyClassA {
public:
    int val1;
    int val2;
};

class MyClassB {
private:
    int val3;

public:
    int val1;
    int val2;

    int MyMethod();
};
```

The compiler would choose to treat `MyClassA` as a structure, because it can. It would not do this to `MyClassB` because it has a method, and it also has private members.
You could not make a structure equivalent because of these things, so it will always be treated as a class.
Since the compiler can be aware of the optimization to make `MyClassA` a structure behind the scenes, the developer **cannot** explicitly declare a structure.

There is no benefit to being able to choose between using a class versus a structure, because **logically** (the way they are used) structures are useless.
They are very basic classes that only exist because of the fact that they are closer to the machine. This is why some assembly languages and C have them, but not classes.
**Physically** (on the hardware level, for speed and space efficiency) there is a benefit to structures, but the developer should not care, hence the compiler doing these optimizations without the input of the developer.

\* With the exception of low-level tasks that require maximum efficiency or low-level features, e.g. using assembly language ever.

___

## Consistency

A big issue with many languages is consistency in how different objects are treated. For example, int C# depending on what the object is, it will either be passed by value or by reference as a default.
This is an issue, because it forces the developer to know which objects are passed by value by default, versus by reference by default.
This is a small thing, but no matter the object (including C primitive objects like `int`) they will be passed by value. You can then specify that it is a reference.

Another point on C# is the use of a modifier (`?`) on a value type to make it nullable. This adds unnecessary complexity. All objects in BELTE are nullable.

___

## DataTypes

Some of the biggest changes with BELTE from other languages are the unique set of provided types.

### NULL

Null can be used on any object, and it prevents using the object (calling methods, accessing members). Note that null is **not** the same as 0, false, empty string, etc.

### Pointers

C-style pointers are not available. References are safer and more intuitive.

### Numbers

Instead of having different types of numbers depending on capacity, there will be different number objects purely based on fundamental differences.

| Type | Description |
|-|-|
| int | any whole number, no limit |
| float | any decimal number, no limit |
| uint | and whole number >= 0, no limit |

This list is short, because it is all that is needed.
You can add on to each of these base types a range on what numbers they can contain, allowing developers to have a limit on a number in niche situations where it is needed.

### Enumerables

Array versus vector versus linked list. They are used identically, but because of efficiency are used in different contexts.
Instead, the compiler will choose, and switch dynamically if needed.

| Type | Description |
|-|-|
| Collection/Collect | A mutable, ordered group of objects with a shared type, duplicates allowed |
| Set | A mutable, unordered group of objects with a shared type, with no duplicates |
| Map/Dictionary | A mutable, unordered\* group of key-value pairs, no duplicate keys |

\* A map is ordered based on keys under the hood unless unable (if the key is a `string` for example) to allow an algorithm to find them without iterating through every key (hashmap).

### Structures

The one use of structures specifically over classes is anonymous structures for parameters in functions (usually).
To solve this a C#-style `tuple` will be used instead because they are cleaner, more intuitive, and structures will never be added to BELTE.