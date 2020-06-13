import app from './plugins/appstart';
import Start from './plugins/vuemixin';
import bootstrap from 'bootstrap';
import prismEditor from 'vue-prism-editor';
import AddToProjectModalComponent from './components/AddToProjectModal.vue';
import vuetag from '@johmun/vue-tags-input';
import { VueEditor } from "vue2-editor";

// start mixin
Start();


window.VueApp = {};

window.VueApp.Components = {
    prismEditor,
    AddToProjectModalComponent,
    VueEditor,
    vuetag
};

window.VueApp.Start = app.Start; 
window.VueApp.Mode = process.env.NODE_ENV;