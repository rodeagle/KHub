import Vue from 'vue';
import axios from 'axios';

export default function Init() {
    Vue.mixin({
        data: function () {
            return {
                g_baseUrl: '',
                g_AntiForgeryKey : ''
            }
        },
        methods: {
            IsValidEmail: function (email) {
                let re = /\S+@@\S+\.\S+/;
                re.test(email);
            },
            RedirectToUrl: function (url) {
                window.location = url;
            },
            Validate: function (validation) {
                let valid = Object.keys(validation).every(function (key) {
                    return validation[key];
                });
                return valid;
            },
            CopyToClipboard: function (text) {

                var dummy = document.createElement("input");
                // to avoid breaking orgain page when copying more words
                // cant copy when adding below this code
                // dummy.style.display = 'none'
                document.body.appendChild(dummy);
                //Be careful if you use texarea. setAttribute('value', value), which works with "input" does not work with "textarea". – Eduard
                dummy.value = text;
                dummy.select();
                dummy.setSelectionRange(0, 99999); /*For mobile devices*/
                document.execCommand("copy");
                document.body.removeChild(dummy);

            },
            UpdateProject: function (projectid, members, userEdited, icon, color) {

                var model = {
                    ProjectID: projectid,
                    EditedUserIDs: members,
                    UserEdited: userEdited,
                    Icon: icon,
                    Color : color
                };

                return new Promise((resolve, reject) => {
                    console.log('updated project');
                    axios.post('/Home/UpdateProject', model)
                        .then(function (response) {
                            response.data.success && resolve();
                            !response.data.success && reject(response.data.message);
                        })
                        .catch(function (error) {
                            reject(error.message);
                        });
                });
            },
            AddPostToProject: function (postid, projectid) {

                var model = {
                    postid: postid,
                    projectid: projectid
                };

                return new Promise((resolve, reject) => {
                    console.log('add to project');
                    axios.post('/Home/AddPostToProject', model)
                        .then(function (response) {
                            response.data.success && resolve();
                            !response.data.success && reject(response.data.message);
                        })
                        .catch(function (error) {
                            reject(error.message);
                        });
                });
            },
            AddPostToFavorites: function (postid) {

                var model = {
                    postid: postid
                };

                return new Promise((resolve, reject) => {
                    console.log('add to favorites');
                    axios.post('/Home/AddPostToFavorites', model)
                        .then(function (response) {
                            response.data.success && resolve();
                            !response.data.success && reject(response.data.message);
                        })
                        .catch(function (error) {
                            reject(error.message);
                        });
                });
            },
            SignIn: function (alias, password) {
                return new Promise((resolve, reject) => {
                    console.log('signe in');
                    axios.post('/Home/SignIn',
                        {
                            alias: alias,
                            password: password
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
            CreateAccount: function (alias,email, password) {
                return new Promise((resolve, reject) => {
                    axios.post('/Home/CreateAccount',
                        {
                            alias: alias,
                            email : email,
                            password: password
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
            },
            CreatePost: function (data) {

                console.log('Vuemixn : CreatePost');
                console.log(data);

                var model = {
                    title: data.title,
                    description: data.description,
                    //tags: data.tags,
                    projectid: data.selected,
                    codes: data.codes
                };
                return new Promise((resolve, reject) => {
                    axios.post('/Home/CreatePost', model)
                        .then(function (response) {
                            response.data.success && resolve(response.data);
                            !response.data.success && reject(response.data.message);
                        })
                        .catch(function (error) {
                            reject(error.message);
                        });
                });
            },
            GetUserProjects: function () {

                return new Promise((resolve, reject) => {
                    axios.post('/Home/GetUserProjects')
                        .then(function (response) {
                            response.data.success && resolve(response.data);
                            !response.data.success && reject(response.data.message);
                        })
                        .catch(function (error) {
                            reject(error.message);
                        });
                });

            }
        }
    });
};