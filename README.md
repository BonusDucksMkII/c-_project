# Overview

This is a simple encryption/decryption program for files. It takes an input and output file and then encrypts or decrypts.

I wrote this to learn some C#, and to learn more about cryptographic algorithms in the context of Computer Science.

[Software Demo Video](https://youtu.be/DvSYIDJjXr8)

# Development Environment

I wrote this in C#, .NET 5.0 (I was lazy, didn't want to download the newer platform) using VSCode.

This also uses C#'s System.Security.Cryptography classes, like CryptoStream and SymmetricAlgorithm.

# Useful Websites

{Make a list of websites that you found helpful in this project}

- [Microsoft's Own .NET API Docs](https://learn.microsoft.com/en-us/dotnet/api/)
- [Stack Overflow, For Debugging Questions](https://stackoverflow.com/questions/8583112/padding-is-invalid-and-cannot-be-removed)

# Future Work

{Make a list of things that you need to fix, improve, and add in the future.}

- Implement working padding on cypher blocks.
- Look into files other than .txt files.