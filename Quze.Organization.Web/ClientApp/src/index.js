import 'jquery';
import "bootstrap/dist/js/bootstrap.bundle.min.js"
import 'popper.js';
import 'bootstrap';
import "bootstrap/dist/js/bootstrap.min.js"
import 'bootstrap/dist/css/bootstrap.css';
import '../src/css/site.css';
import '../src/css/general.css';
import '../src/css/login.css';
import '../src/css/demo.css';
import '../src/css/pe-icon-7-stroke.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { withI18n } from "react-i18next";




const rootElement = document.getElementById('root');
const AppWithI18n = withI18n()(App);
ReactDOM.render(<AppWithI18n />, rootElement);

registerServiceWorker();
