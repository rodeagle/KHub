<template>
    <header>
        <modals-container />
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
                        <v-btn icon  class="d-sm-block d-xs-block d-md-none">
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

    import vuetag from '@johmun/vue-tags-input';
    import VuePrismEditor from "vue-prism-editor";
    import $ from 'jquery';

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
                    {
                        // to add later on template vuetags
                            /*<label class="font-weight-bold">Tags</label>*/
                            /*<small>What defines the solution (multiple answers)</small>*/
                            /*<vue-tag-input v-model="Tags"></vue-tag-input>*/
                        template: `
                        <div class="p-3">
                            <h2>Create New Post</h2>
                            <label class="font-weight-bold">Title</label>
                            <small>Make it sound and clear</small><br>
                            <input class="form-control" v-model="title"/>
                            <div v-show="!!!createPostValidation['Title.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                            <label class="font-weight-bold">Project</label>
                            <small>(it will inherit the projects visibility)</small>
                            <select v-model="selected" class="form-control" placeholder="Select a Project">
                              <option v-for="option in options" v-bind:value="option.value">
                                {{ option.text }}
                              </option>
                            </select>
                            <div v-show="!!!createPostValidation['Project.Required']" class="text-danger">You must select a project first</div>
                            <label class="font-weight-bold">Description</label>
                            <small>Add instructions or a brief description (Accepts HTML)</small>
                            <textarea rows="10" class="w-100 form-control" v-model="description"></textarea><br>
                            <div v-show="!!!createPostValidation['Description.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                            <label class="font-weight-bold">Code</label>
                            <small>Code section to attach</small>
                            <prism-editor :code="code" v-model="code" language="js" :line-numbers="true"></prism-editor>
                            <br>
                            <div class="row">
                                <div class="col-12 col-sm-12 col-lg-6">
                                    <v-btn class="btn-block"  color="secondary" @click="_CloseModal">Cancel</v-btn>
                                </div>
                                <div class="col-12 col-sm-12 col-lg-6">
                                    <v-btn class="btn-block" :loading="createProjectSubmitting" :disabled="createProjectSubmitting" color="primary" @click="_CreatePost">Create</v-btn>
                                </div>
                            </div>
                        </div>
                        `,
                        //props: [],
                        components: {
                            'vue-tag-input': vuetag,
                            'prism-editor': VuePrismEditor,
                        },
                        data: function () {
                            return {
                                // create project
                                createPostValidation: {
                                    "Title.Required": true,
                                    "Description.Required": true,
                                    "Project.Required": true
                                },
                                title: '',
                                tags: [],
                                createPostSubmitting: false,
                                selected: '',
                                options: [
                                  { text: 'Uno', value: 'A' },
                                  { text: 'Dos', value: 'B' },
                                  { text: 'Tres', value: 'C' }
                                ], 
                                code: '',
                                description : '',
                            };
                        },
                        mounted: function () {
                            let $this = this;
                            this.GetUserProjects()
                                .then(function (response) {
                                    var mapped = response.result.map(function (x) {
                                        return { value: x.projectID, text: x.name }
                                    });
                                    $this.$data.options = [{ value: '', text : '-- Select a Project --'},...mapped]; 
                                });    
                        },
                        methods: {
                            _CloseModal: function () {
                                this.$emit('close');
                            },
                            _CreatePost: function () {

                                this.createPostValidation = {
                                    "Title.Required": !!this.title && this.title.length > 4,
                                    "Description.Required": !!this.description && this.description.length > 4,
                                    "Project.Required": !!this.selected
                                };

                                let _validation = this.createPostValidation;
                                let valid = Object.keys(_validation).every(function (key) {
                                    return _validation[key];
                                });

                                if (!valid) {
                                    return;
                                }

                                this.createPostSubmitting = true;

                                this.CreatePost(this.$data)
                                    .then(() => {
                                        this.$emit('close');
                                        this.$notify({
                                            title: 'Success:',
                                            text: "Created Post successfully",
                                            type: 'success',
                                        });
                                        //setTimeout(() => {
                                        //    location.reload();
                                        //}, 1000);
                                    }).catch((error) => {
                                        this.$notify({
                                            title: 'Error:',
                                            text: error,
                                            type: 'error',
                                        });
                                        this.createProjectSubmitting = false;
                                    });

                            }
                        }
                    },
                    {
                        //text: 'This text is passed as a property'
                    },
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
                    {
                        template: `
                        <div class="p-3">
                            <h2>Create Project</h2>
                            <label class="font-weight-bold">Project Name</label><br>
                            <input name="ProjectName" class="form-control" v-model="projectName"/><br>
                            <div v-show="!!!createProjectValidation['ProjectName.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                            <v-switch flat dense v-model="projectIsPublic" color="success" inset :label="'Make the project public'"></v-switch>
                            <div class="text-right">
                                <v-btn class="btn-block" :loading="createProjectSubmitting" :disabled="createProjectSubmitting" color="primary" @click="_CreateProject">Create</v-btn>
                            </div>
                        </div>
                        `,
                        //props: [],
                        data: function () {
                            return {
                                // create project
                                createProjectValidation: {
                                    "ProjectName.Required": true
                                },
                                projectName: '',
                                projectIsPublic: true,
                                createProjectSubmitting: false
                            };
                        },
                        methods: {
                            _CreateProject: function () {

                                this.createProjectValidation = {
                                    "ProjectName.Required": !!this.projectName && this.projectName.length > 4,
                                };

                                let _validation = this.createProjectValidation;
                                let valid = Object.keys(_validation).every(function (key) {
                                    return _validation[key];
                                });

                                if (!valid) {
                                    return;
                                }

                                this.createProjectSubmitting = true;

                                this.CreateProject(this.projectName, this.projectIsPublic)
                                    .then(() => {
                                        this.$emit('close');
                                        this.$notify({
                                            title: 'Success:',
                                            text: "Created Project successfully",
                                            type: 'success',
                                        });
                                        //setTimeout(() => {
                                        //    location.reload();
                                        //}, 1000);
                                    }).catch((error) => {
                                        this.$notify({
                                            title: 'Error:',
                                            text: error,
                                            type: 'error',
                                        });
                                        this.createProjectSubmitting = false;
                                    });

                            }
                        }
                    },
                    {
                        //text: 'This text is passed as a property'
                    },
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
                    {
                        template: `
                        <div class="p-3 login-modal-template">
                            <div class="row">
                                <div class="col-sm-12 col-md-6 pb-3 pr-md-5 render-divider-right">
                                    <h2>Login</h2>
                                    <label class="font-weight-bold">Login Name</label><br>
                                    <input name="LoginName" class="form-control" v-model="localAlias"/><br>
                                    <div v-show="!!!validation['LoginName.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                                    <label class="font-weight-bold">Password</label><br>
                                    <input name="Password" class="form-control" v-model="localPassword"/><br>
                                    <div v-show="!!!validation['LoginName.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                                    <div class="text-right">
                                        <v-btn class="btn-block" :loading="signInSubmitting" :disabled="signInSubmitting" color="primary" @click="_SignIn">Login</v-btn>
                                    </div>
                                </div>
                                <div class="col-sm-12 col-md-6 pb-3 pl-md-5">
                                    <h2>Create Account</h2>
                                    <label class="font-weight-bold">Login Name</label><br>
                                    <input name="LoginName" class="form-control" v-model="createAlias"/><br>
                                    <div v-show="!!!createValidation['LoginName.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                                    <label class="font-weight-bold">Password</label><br>
                                    <input name="Password" class="form-control" v-model="createPassword"/><br>
                                    <div v-show="!!!createValidation['Password.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                                    <div class="text-right">
                                        <v-btn class="btn-block" :loading="createAccountSubmitting" :disabled="createAccountSubmitting" color="primary" @click="_CreateAccount">Create Account</v-btn>
                                    </div>
                                </div>
                            </div>
                        </div>
                        `,
                        props: ['text', 'Alias', "Password"],
                        data: function () {
                            return {
                                validation: {
                                    "LoginName.Required": true,
                                    "Password.Required": true
                                },
                                createValidation: {
                                    "LoginName.Required": true,
                                    "Password.Required": true
                                },
                                localAlias: this.Alias,
                                localPassword: this.Password,
                                createAlias: '',
                                createPassword: '',
                                signInSubmitting: false,
                                createAccountSubmitting: false,
                            };
                        },
                        methods: {
                            _SignIn: function () {
                                this.signInSubmitting = true;
                                // validate
                                this.validation = {
                                    "LoginName.Required": !!this.localAlias && this.localAlias.length > 4,
                                    "Password.Required": !!this.localAlias && this.localAlias.length > 4,
                                };

                                let _validation = this.validation;
                                let valid = Object.keys(_validation).every(function (key) {
                                    return _validation[key];
                                });

                                if (!valid) {
                                    this.signInSubmitting = false;
                                    return;
                                }

                                this.SignIn(this.localAlias, this.localPassword)
                                    .then(() => {
                                        this.$emit('close');
                                        this.$notify({
                                            title: 'Success:',
                                            text: "Login successful",
                                            type: 'success',
                                        });
                                        setTimeout(() => {
                                            location.reload();
                                        }, 1000);
                                    }).catch(error => {
                                        this.$notify({
                                            title: 'Error:',
                                            text: error,
                                            type: 'error',
                                        });
                                        this.signInSubmitting = false;
                                    });
                            },
                            _CreateAccount: function () {
                                debugger;
                                this.createAccountSubmitting = true;

                                this.createValidation = {
                                    "LoginName.Required": !!this.createAlias && this.createAlias.length > 4,
                                    "Password.Required": !!this.createPassword && this.createPassword.length > 4,
                                };

                                let _validation = this.createValidation;
                                let valid = Object.keys(_validation).every(function (key) {
                                    return _validation[key];
                                });

                                if (!valid) {
                                    this.createAccountSubmitting = false;
                                    return;
                                }

                                this.CreateAccount(this.createAlias, this.createPassword)
                                    .then(() => {
                                        this.$emit('close');
                                        this.$notify({
                                            title: 'Success:',
                                            text: "Created Account successfully",
                                            type: 'success',
                                        });
                                        setTimeout(() => {
                                            location.reload();
                                        }, 1000);
                                    }).catch((error) => {
                                        this.$notify({
                                            title: 'Error:',
                                            text: error,
                                            type: 'error',
                                        });
                                        this.createAccountSubmitting = false;
                                    });
                            }
                        }
                    },
                    {
                        text: 'This text is passed as a property'
                    },
                    {
                        height: 'auto',
                        adaptive: true,
                        //width: 'auto'
                    },
                    //{
                    //    'before-close': (event) => { console.log('this will be called before the modal closes'); },
                    //}
                );
            }
        },
        mounted: function () {

        }
    }
</script>

