import app from './plugins/appstart';
import Start from './plugins/vuemixin';
import bootstrap from 'bootstrap';

// start mixin
Start();


window.VueApp = {};

window.VueApp.Components = {

};

window.VueApp.Start = app.Start; 
window.VueApp.Mode = process.env.NODE_ENV;