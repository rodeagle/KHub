import app from './plugins/appstart';
import Start from './plugins/vuemixin';
import bootstrap from 'bootstrap';
import CodeEditor from './components/CodeEditor.vue';

// start mixin
Start();


window.VueApp = {};

window.VueApp.Components = {
    CodeEditor : CodeEditor
};

window.VueApp.Start = app.Start; 
window.VueApp.Mode = process.env.NODE_ENV;