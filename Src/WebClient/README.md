<h1 align="center">
  <br>
  <a href="https://dualog.net"><img src="http://dualog.com/sites/dualog.com/themes/dualog/logo.png" alt="Dualog" width="200"></a>
  <br>
    dualog.net-UI
  <br>
</h1>

# Introduction
dualog.net-UI is the shore-side user interface for Dualog Connection Suite. 

See the bluebird website for further documentation, references and instructions. 
See the [**API reference**](http://devtask.dualog.net:84/swagger/) here.

# Questions and issues
The github issue tracker is only for bug reports and feature requests. Anything else, such as questions for help in using the library, should be posted in Aha.


# How To Use
To clone and run this application, you'll need [Git](https://git-scm.com) and [Node.js](https://nodejs.org/en/download/) (which comes with [npm](http://npmjs.com)) installed on your computer. From your command line:

```bash
# Clone this repository
$ git clone https://github.com/Dualog/dualog.net-UI.git

# Go into the repository
$ cd dualog.net-UI.git

# Install dependencies
$ npm install

# Run the app
$ npm start

# If it doesn't start, there are some bug in a version with angular-cli version. 
# Start the applications by this command: 
$ ng serve --host [your IP] --port [some port]
```

Note: The host and the port must exist in the database AP_IDENTITYCLIENT and AP_IDENTITYCLIENTURL.


