# Project structure

Names shown in _italic_ are concrete file system folder names.

This project has the following high-level source code structure:

* _infrastructure_ [contains generic infrastructure code with dependencies only to angular and other third party libraries]
  - _ui_ [contains generic ui components]
  - _services_ [contains generic interfaces, classes and angular services]
  - _directives_ [contains angular directives]

  Each folder may contain further subfolders for different concepts.

* _common_ [contains Dualog common code]
  - _domain_ [contains models and service classes]
  - _services_ [contains common interfaces, classes and angular services]
  - _ui_ [contains common user interface components]

  Each folder may contain further subfolder for different concepts.
  May only have dependencies to _infrastructure_.

* _shore_ [contains Dualog Connection Suite shore applicaion]
  - _app_ [contains angular application classes]
  - _domain_ [contains shore specific models and services]
  - _ui_ [contains shore specific user interface components]
  - _services_ [contains shore specific interfaces, classes and angular services]

  Each folder may contain further subfolder for different concepts.
  May only have dependencies to _infrastructure_ and _common_.
