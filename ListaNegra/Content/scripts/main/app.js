define(['vue', 'vuetify', 'input-mask', 'jquery','notifications', 'vueLoader!components/InputImageHidden.vue'],
    function (Vue, Vuetify, VueMask, $, notifications) {
    //add the vue plugin on start up
    Vue.use(Vuetify);
    Vue.use(VueMask.default);
    Vue.use(notifications.default);
    console.log("Vuetify Directive loaded:", Vuetify);
    console.log("VueMask Directive loaded:", VueMask);
    console.log("notifications Directive loaded:", notifications);

    //Vue Components preloaded with requirejs

    var app = {
        vuetify: new Vuetify(),
    };

    return {
        Start: function (settings) {
            app = $.extend({}, app, settings);
            if (app.data) {
                app.data['isMounted'] = false;
            } else {
                app.data = { isMounted : false };
            }

            let $app = new Vue(app).$mount('#MainApp');

            setTimeout(() => {
                $app.$data.isMounted = true;

            }, 100);
        }
    };
});
