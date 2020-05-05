<template>
    <div v-show="url.length > 0">
        <small>Imagenes agregadas</small>
        <div v-for="(item,index) in url" class="input-group">
            <input type="text" class="form-control form-readonly" :value="item" readonly/>
            <v-btn fab class="secondary" x-small  color="error"  v-on:click="remove(index)">
                <v-icon>remove_circle_outline</v-icon>
            </v-btn>
        </div>
    </div>
</template>

<style scoped>
    .form-readonly{
        border-radius:0;
        border:0;
        text-decoration: underline;
        background-color:transparent !important;
        outline: none;
    }

    .for-readonly:focus{
       outline:none;
      }
</style>

<script>
    define(['vue'], function(Vue){
        Vue.component('input-image-hidden', {
            template: template,
            props: {
                url: Array
            },
            methods: {
                remove: function (item) {
                    console.log(item);
                    console.log(this.url);
                    this.url = this.url.splice(item,1);
                    console.log(this.url);
                }
            }
        });
    });
</script>