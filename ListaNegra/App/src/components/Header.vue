<template>
    <header>
        <modals-container />
        <div>
            <v-app-bar dark
                       app
                       collapse-on-scroll>
                <v-app-bar-nav-icon @click="drawer = true"></v-app-bar-nav-icon>

                <v-toolbar-title class="font-italic">K.Hub	&nbsp;	&nbsp;</v-toolbar-title>

                <v-spacer></v-spacer>

                <v-btn icon>
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

                <v-tooltip bottom v-if="issignedin">
                    <template v-slot:activator="{ on }">
                        <v-btn icon v-on="on">
                            <v-icon>mdi-heart</v-icon>
                        </v-btn>
                    </template>
                    <span>See your favorite's contributions</span>
                </v-tooltip>

                <v-btn v-if="issignedin">
                    {{alias}}
                </v-btn>
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
                                <v-icon>mdi-home</v-icon>
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
        <v-fab-transition class="pb-12" v-if="issignedin">
            <v-tooltip top v-if="issignedin">
                <template v-slot:activator="{ on }">
                    <v-btn color="pink"  v-on="on" dark fixed bottom right fab @click="CreatePost">
                        <v-icon>mdi-plus</v-icon>
                    </v-btn>
                </template>
                <span>Add a new contribution!</span>
            </v-tooltip>
        </v-fab-transition>
    </header>
</template>

<script>

    import vuetag from '@johmun/vue-tags-input';

    export default {
        props: {
            issignedin: Boolean,
            alias: String
        },
        data: function () {
            return {
                'drawer': false
            };
        },
        methods: {
            CreatePost: function () {
                this.$modal.show(
                    {
                        template: `
                        <div class="p-3">
                            <h2>Create New Post</h2>
                            <label class="font-weight-bold">Name</label><br>
                            <input name="Name" class="form-control" v-model="Name"/><br>
                            <div v-show="!!!createPostValidation['Name.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                            <v-switch flat dense v-model="projectIsPublic" color="success" inset :label="'Make the project public'"></v-switch>
                            <div class="text-right">
                                <v-btn class="btn-block" :loading="createProjectSubmitting" :disabled="createProjectSubmitting" color="primary" @click="_CreateProject">Create</v-btn>
                            </div>
                        </div>
                        `,
                        //props: [],
                        components: {
                            'vue-tag-input': vuetag,
                        },
                        data: function () {
                            return {
                                // create project
                                createPostValidation: {
                                    "ProjectName.Required": false
                                },
                                Name: '',
                                Tags: true,
                                createPostSubmitting: false
                            };
                        },
                        methods: {
                            _CreatePost: function () {

                                this.createPostValidation = {
                                    "Name.Required": !!this.Name && this.Name.length > 4,
                                };

                                let _validation = this.createPostValidation;
                                let valid = Object.keys(_validation).every(function (key) {
                                    return _validation[key];
                                });

                                if (!valid) {
                                    return;
                                }

                                this.createPostSubmitting = true;

                                this.CreatePost(this.name, )
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
                        adaptive: true
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
                        adaptive: true
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
                                    <div v-show="!!!createValidation['LoginName.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
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
                        adaptive: true
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

