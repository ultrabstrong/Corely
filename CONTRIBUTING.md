# Contributing to Corely

We welcome contributions to the Corely library! Here are some guidelines to help you get started.

## Table of Contents
- [How to Contribute](#how-to-contribute)
- [Reporting Issues](#reporting-issues)
- [Coding Standards](#coding-standards)
  - [General Guidelines](#general-guidelines)
  - [Naming Conventions](#naming-conventions)
    - [Containing Types](#containing-types)
    - [Members](#members)
    - [Usage-Based Naming](#usage-based-naming)
  - [Coding Style](#coding-style)
  - [Security](#security)
- [License](#license)

## How to Contribute

1. **Clone the Repository**: Clone the repository to your local machine.
   
2. **Create a Branch**: Create a new branch for your feature or bug fix.
   
3. **Make Changes**: Make your changes to the codebase. Ensure that your code follows the project's coding standards and includes appropriate tests.

4. **Commit Changes**: Commit your changes with a descriptive commit message.
   
5. **Push Changes**: Push your changes to the repository.
   
6. **Create a Pull Request**: Open a pull request to the main repository. Provide a clear description of your changes and any related issues.

## Reporting Issues

If you find a bug or have a feature request, please open an issue on the [GitHub Issues](https://github.com/ultrabstrong/Corely/issues) page.

## Coding Standards

### General Guidelines
- Domain-agnostic code should be put in the `Corely.Common` project
- Use comments sparingly, if ever; only when necessary
- Prefer json over xml when possible

### Naming Conventions

#### Containing Types
- One class / enum / interface per file
- Prefix interface names with 'I'
- Postfix abstract class names with 'Base'

#### Members
- `UsePascalCase` for class, method, and property names
- `useCamelCase` for local variables
- `_useThisCamelCase` for private variables
- `USE_SCREAMING_SNAKE_CASE` for constants
- Use 'Async' as a suffix for asynchronous methods

#### Usage-Based Naming
- `Provider` for classes that provide "standard" (i.e. non-domain specific) functionality (i.e. encryption, hashing, sftp, ftp, file operations, encoders, etc.)
- `Service` for classes that provide top-level coordination of business logic
  - Services should be publicly scoped
- `Processor` for classes that implement business logic
  - Processors should be internally scoped
- `Repo` for classes that act as a repository
  - Repositories should be internally scoped
  - `Corely.DataAccess` interfaces and base classes should be used for repositories
- `Model` for domain model or provider model
- `Entity` for database-related data objects
- `DTO` for data objects that transfer data between layers of the application

### Coding Style
- Write unit tests when applicable
  - Prefer XUnit, Moq, and AutoFixture frameworks
- Follow DDD and SOLID principles

### Security
- Use `Corely.Security` for encryption and hashing
- Always use `ISymmetricEncryptedValue` or `IAsymmetricEncryptedValue` instead of storing a decrypted value in a string
- Don't include encryption keys in code (provision with `ISymmetricKeyStoreProvider` and `IAsymmetricKeyStoreProvider`)

## License

By contributing to Corely, you agree that your contributions will be licensed under the MIT License.

Thank you for contributing!