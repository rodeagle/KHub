import Vue from 'vue';
import axios from 'axios';

export default function Init() {
    Vue.mixin({ 
        methods: {
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
            AddPostToFavorites: function (postid) {
                return new Promise((resolve, reject) => {
                    console.log('add to favorites');
                    axios.post('/Home/AddPostToFavorites',
                        {
                            postid : postid
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
            CreateAccount: function (alias, password) {
                return new Promise((resolve, reject) => {
                    axios.post('/Home/CreateAccount',
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
                    code: data.code
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