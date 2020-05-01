requirejs.config({
    baseUrl: 'Scripts',
    paths: {
        'jquery': 'https://code.jquery.com/jquery-3.4.1.min',
        'bootstrap': 'https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.bundle.min',
        'openlayers': 'https://cdn.jsdelivr.net/gh/openlayers/openlayers.github.io@master/en/v6.3.1/build/ol',
        'vue': 'https://cdnjs.cloudflare.com/ajax/libs/vue/2.6.11/vue',
        'vuetify' : 'https://cdn.jsdelivr.net/npm/vuetify@2.x/dist/vuetify',
        'tag-input-vue': 'https://unpkg.com/@johmun/vue-tags-input/dist/vue-tags-input'
    },
    shim: {
        'vuetify': {
            deps : ['vue']
        },
        "tag-input-vue": {
            deps: ['vue', 'vuetify']
        }
    }
});

require(['jquery', 'bootstrap']);

