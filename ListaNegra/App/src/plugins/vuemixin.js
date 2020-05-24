import Vue from 'vue';
import axios from 'axios';

export default function Init() {
    Vue.mixin({
        methods: {
            SignIn: function (alias, password) {
                return new Promise((resolve, reject) => {
                    console.log('signe in');
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
};