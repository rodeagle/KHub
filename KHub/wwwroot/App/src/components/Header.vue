<template>
    <header>
        <modals-container />
        <v-dialog />
        <div>
            <v-app-bar dark
                       app
                       :collapse-on-scroll="!search">
                <!--<v-app-bar-nav-icon @click="drawer = true"></v-app-bar-nav-icon>-->

                <a :href="baseurl" class="clickable-anchor">
                    <v-toolbar-title class="font-italic">Knowledge Hub 	&nbsp;	&nbsp;</v-toolbar-title>
                </a>

                <v-spacer></v-spacer>

                <div v-show="search"><input class="form-control d-none d-md-block d-lg-block d-xl-block" @keyup.enter="Search" v-model="searchInput" /></div>

                <v-btn icon @click="ToggleSearch">
                    <v-icon>mdi-magnify</v-icon>
                </v-btn>

                <v-tooltip bottom v-if="issignedin">
                    <template v-slot:activator="{ on }">
                        <v-btn icon v-on="on" @click="CreateProjectModal">
                            <v-icon>create_new_folder</v-icon>
                        </v-btn>
                    </template>
                    <span>Add a new project</span>
                </v-tooltip>



                <v-btn class="d-none d-md-block d-lg-block d-xl-block" v-if="issignedin">
                    {{alias}}
                </v-btn>
                <v-tooltip bottom v-if="issignedin">
                    <template v-slot:activator="{ on }">
                        <v-btn icon class="d-sm-block d-xs-block d-md-none">
                            <v-icon v-on="on">account_circle</v-icon>
                        </v-btn>
                    </template>
                    <span>{{alias}}</span>
                </v-tooltip>
                <v-btn icon v-if="!issignedin" @click="StartLoginProcess">
                    <v-icon>account_circle</v-icon>
                </v-btn>
            </v-app-bar>
            <v-navigation-drawer v-model="drawer"
                                 absolute
                                 temporary>
                <v-list nav
                        dense>
                    <v-list-item-group v-model="group"
                                       active-class="deep-purple--text text--accent-4">
                        <v-list-item>
                            <v-list-item-icon>
                                <v-btn href="">
                                    <v-icon>mdi-home</v-icon>

                                </v-btn>
                            </v-list-item-icon>
                            <v-list-item-title>Home</v-list-item-title>
                        </v-list-item>

                        <v-list-item>
                            <v-list-item-icon>
                                <v-icon>mdi-account</v-icon>
                            </v-list-item-icon>
                            <v-list-item-title>Account</v-list-item-title>
                        </v-list-item>

                    </v-list-item-group>
                </v-list>
            </v-navigation-drawer>
        </div>
        <!--v-show="ToggleSearch"-->
        <!--d-none d-xs-block d-sm-block-->
        <div v-show="search" class="toggable-search-mobile">
            <div class="mobile-search-input d-xs-block d-sm-block d-md-none">
                <input type="text" class="form-control" placeholder="Search...." v-model="searchInput" @keyup.enter="Search" />
            </div>
        </div>
        <v-fab-transition class="pb-12" v-if="issignedin">
            <v-tooltip top v-if="issignedin">
                <template v-slot:activator="{ on }">
                    <v-btn color="pink" v-on="on" dark fixed bottom right fab @click="CreatePost">
                        <v-icon>mdi-plus</v-icon>
                    </v-btn>
                </template>
                <span>Add a new contribution!</span>
            </v-tooltip>
        </v-fab-transition>
    </header>
</template>

<style>
    .clickable-anchor{
        color:white !important;
        font-family:Impact, Haettenschweiler, 'Arial Narrow Bold', sans-serif;
    }

    .clickable-anchor:hover{
        text-decoration:none;
    }
</style>

<script>

    //import vuetag from '@johmun/vue-tags-input';
    //import VuePrismEditor from "vue-prism-editor";
    //import $ from 'jquery';
    import LoginModalComponent from '../components/LoginModalComponent.vue';
    import CreatePostModalComponent from '../components/CreatePostModal.vue';
    import CreateProjectModalComponent from '../components/CreateProjectModal.vue';

    export default {
        props: {
            issignedin: Boolean,
            alias: String,
            baseurl : String
        },
        data: function () {
            return {
                'drawer': false,
                'search': false,
                'searchInput': ''
            };
        },
        methods: {
            Search: function () {
                if (this.searchInput == '') {
                    return;
                }
                window.location = this.baseurl + '/Home/Index?search=' + this.searchInput;
            },
            ToggleSearch: function () {
                this.search = !this.search;
            },
            CreatePost: function () {
                this.$modal.show(
                    CreatePostModalComponent,
                    {},
                    {
                        height: 'auto',
                        adaptive: true,
                        classes: 'd-modal',
                        scrollable: true,
                        //clickToClose : false
                    },
                    {}
                );
            },

            CreateProjectModal: function () {
                this.$modal.show(
                    CreateProjectModalComponent,
                    {},
                    {
                        height: 'auto',
                        adaptive: true,
                        clickToClose : false
                    },
                    {}
                );
            },
            StartLoginProcess: function () {
                this.$modal.show(
                    LoginModalComponent,
                    { baseurl: this.baseurl },
                    {
                        height: 'auto',
                        adaptive: true,
                        //width: 'auto'
                    },
                );
            }
        },
        mounted: function () {

        }
    }
</script>

