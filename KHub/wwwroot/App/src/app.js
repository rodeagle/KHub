import app from './plugins/appstart';
import Start from './plugins/vuemixin';
import bootstrap from 'bootstrap';
import prismEditor from 'vue-prism-editor';

// start mixin
Start();


window.VueApp = {};

window.VueApp.Components = {
    prismEditor
};

window.VueApp.Start = app.Start; 
window.VueApp.Mode = process.env.NODE_ENV;