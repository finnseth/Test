# Project structure

Names shown in **`bold`** are concrete file system folder names.

This project has the following high-level source code structure:

* **`infrastructure`** [contains generic infrastructure code with dependencies only to angular and other third party libraries]
  - **`ui`** [contains generic ui components]
  - **`services`** [contains generic interfaces, classes and angular services]
  - **`directives`** [contains angular directives]

  Each folder may contain further subfolders for different concepts.

* **`common`** [contains Dualog common code]
  - **`domain`** [contains models and service classes]
  - **`services`** [contains common interfaces, classes and angular services]
  - **`ui`** [contains common user interface components]

  Each folder may contain further subfolder for different concepts.
  May only have dependencies to **`infrastructure_.

* **`shore`** [contains Dualog Connection Suite shore applicaion]
  - **`app`** [contains angular application classes]
  - **`domain`** [contains shore specific models and services]
  - **`ui`** [contains shore specific user interface components]
  - **`services`** [contains shore specific interfaces, classes and angular services]

  Each folder may contain further subfolder for different concepts.
  May only have dependencies to **`infrastructure`** and **`common`**.
