define(['vue', 'vuetify', 'input-mask', 'jquery', 'notifications', 'vue-modal','axios', 'vueLoader!components/InputImageHidden.vue', 'vueLoader!components/Header.vue'],
    function (Vue, Vuetify, VueMask, $, notifications, vuemodals, axios) {
    //add the vue plugin on start up
    Vue.use(Vuetify);
    Vue.use(VueMask.default);
    Vue.use(notifications.default);
    Vue.use(vuemodals.default, { dynamic: true, /*injectModalsContainer: true */ dynamicDefaults: { clickToClose: false } });
    console.log("Vuetify Directive loaded:", Vuetify);
    console.log("VueMask Directive loaded:", VueMask);
    console.log("notifications Directive loaded:", notifications);
    console.log("vuemodals Directive loaded:", vuemodals);


    //Vue Components preloaded with requirejs

    Vue.mixin({
        //mounted() {
        //    console.log("hello world!");
        //},
        methods: {
            SignIn: function (alias, password) {
                return new Promise((resolve, reject) => {
                    axios.post('/Home/SignIn',
                    {
                        Alias: alias,
                        Password: password
                    })
                    .then(function (response) {
                        response.data.success && resolve();
                        !response.data.success && reject(response.data.message);
                    })
                    .catch(function (error) {
                        reject(error.message);
                    });
                });
            },
            CreateAccount: function (alias, password) {
                return new Promise((resolve, reject) => {
                    axios.post('/Home/CreateAccount',
                        {
                            Alias: alias,
                            Password: password
                        })
                        .then(function (response) {
                            response.data.success && resolve();
                            !response.data.success && reject(response.data.message);
                        })
                        .catch(function (error) {
                            reject(error.message);
                        });
                });
            },
            CreateProject: function (name, isPublic) {
                return new Promise((resolve, reject) => {
                    axios.post('/Home/CreateProject',
                        {
                            name,
                            isPublic: isPublic
                        })
                        .then(function (response) {
                            response.data.success && resolve();
                            !response.data.success && reject(response.data.message);
                        })
                        .catch(function (error) {
                            reject(error.message);
                        });
                });
            }
        }
    });

    var app = {
        vuetify: new Vuetify(),
    };

    return {
        Start: function (settings) {
            app = $.extend({}, app, settings);
            if (app.data) {
                app.data['isMounted'] = false;
                app.data['drawer'] = false;
            } else {
                app.data = { isMounted : false, drawer : false };
            }

            let $app = new Vue(app).$mount('#MainApp');

            setTimeout(() => {
                $app.$data.isMounted = true;

            }, 100);
        }
    };
});
