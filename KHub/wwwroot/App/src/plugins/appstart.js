import Vue from 'vue';
import vuetify from '../plugins/vuetify.js';
import $ from 'jquery';
import header from '../components/Header.vue';
import VModal from 'vue-js-modal';
import Notifications from 'vue-notification';
import axios from 'axios';
import VueAxios from 'vue-axios';
import Prism from 'prismjs';
import VuePrismEditor from "vue-prism-editor";

export default {

    Start: function (settings) {

        console.log(Vue.version);

        Vue.use(VModal, { dialog: true, dynamic: true, /*injectModalsContainer: true */ /*dynamicDefaults: { clickToClose: false }*/ });
        Vue.use(Notifications);
        Vue.use(VueAxios, axios);

        let _globalComponents = {
            'prism-editor' : VuePrismEditor,
            'header-section': header
        };

        let app = {
            vuetify : vuetify
        };
        app = $.extend({}, app, settings);
        if (app.data) {
            app.data['isMounted'] = false;
            app.data['drawer'] = false;
        } else {
            app.data = { isMounted: false, drawer: false };
        }

        if (app.components) {
            app.components = $.extend({}, app.components, _globalComponents );
        } else {
            app.components = _globalComponents;
        }


        Vue.component('header-section', header);

        let $app = new Vue(app).$mount('#MainApp');

        setTimeout(() => {
            $app.$data.isMounted = true;

        }, 100);
    }

}