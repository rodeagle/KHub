define(['vue', 'vuetify', 'input-mask','jquery'], function (Vue, Vuetify, VueMask, $) {
    //add the vue plugin on start up
    Vue.use(Vuetify);
    Vue.use(VueMask);

    Vue.component('input-image-hidden', {
        props: {
            url: Array
        },
        template: `<div>
                        <small>Imagenes agregadas</small>
                        <div v-for="item in url" class="input-group">
                            <input type="textbox" class="form-control" :value="item"/><v-icon class="append-addon clickable">add</v-icon>
                        </div>
                    </div>`,
        methods: {
            remove: function () {
                delete this.url;
            }
        }
    });

    var app = {
        vuetify: new Vuetify()
        //components: {
        //    'manage-add-controller': 'manage-add-controller'
        //}

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
            console.log(1);
            $app.$data.isMounted = true;
            window.addEventListener('hashchange', function () {
                alert('test');
                $app.$data.isMounted = false;
            }, false);
        }
    };
});
