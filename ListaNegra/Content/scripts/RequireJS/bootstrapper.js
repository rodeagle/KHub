requirejs.config({
    baseUrl: '../Content',
    paths: {
        'jquery': 'https://code.jquery.com/jquery-3.4.1.min',
        'bootstrap': 'https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.bundle.min',
        'openlayers': 'https://cdn.jsdelivr.net/gh/openlayers/openlayers.github.io@master/en/v6.3.1/build/ol',
        'vue': 'https://cdnjs.cloudflare.com/ajax/libs/vue/2.6.11/vue',
        'vuetify': 'https://cdn.jsdelivr.net/npm/vuetify@2.x/dist/vuetify',
        'tag-input-vue': 'https://unpkg.com/@johmun/vue-tags-input/dist/vue-tags-input',
        'input-mask': 'https://cdn.jsdelivr.net/npm/vue-the-mask@0.11.1/dist/vue-the-mask.min',
        'app': 'scripts/main/app',
        'vueLoader': 'https://rawgit.com/vikseriq/requirejs-vue/master/requirejs-vue',
        'notifications': 'https://unpkg.com/vue-notification@1.3.20/dist/index',
        'jquery-validate': 'https://cdn.jsdelivr.net/jquery.validation/1.16.0/jquery.validate.min',
        'jquery-validate-unobtrusive': 'https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.11/jquery.validate.unobtrusive.min',
        'vue-modal': 'https://cdn.jsdelivr.net/npm/vue-js-modal@1.3.28/dist/index.min',
        'axios': 'https://cdnjs.cloudflare.com/ajax/libs/axios/0.19.2/axios.min',
        'CreatePostModal' : 'components/CreatePostModal.vue'
    },
    shim: {
        "vue": { "exports": "Vue" },
        'vuetify': {
            deps: ['vue']
        },
        "tag-input-vue": {
            deps: ['vue', 'vuetify']
        },
        'input-mask': {
            deps: ['vue']
        },
        'app': {
            deps: ['vue', 'vuetify', 'input-mask']
        },
        'vueLoader': {
            'pug': 'browser-pug',
            'css': 'inject',
            'templateVar': 'template'
        },
        'notifications': {
            deps: ['vue'],
            exports: 'Notifications'
        },
        'jquery-validate': {
            deps : ['jquery']
        },
        'jquery-validate-unobtrusive': {
            deps: ['jquery', 'jquery-validate']
        }
    }
});


