<template>

    <div class="p-3 login-modal-template">
        <div class="row">
            <div class="col-sm-12 col-md-6 pb-3 pr-md-5 render-divider-right">
                <h2>Login</h2>
                <label class="font-weight-bold">Login Name</label><br>
                <input name="LoginName" class="form-control" v-model="localAlias" /><br>
                <div v-show="!!!validation['LoginName.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                <label class="font-weight-bold">Password</label><br>
                <input name="Password" class="form-control" v-model="localPassword" /><br>
                <div v-show="!!!validation['LoginName.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                <br />
                <a :href="baseurl+'/reset-password'">Forgot your password? Click Here</a>
                <br />
                <br />
                <br />
                <!--buttons-->
                <div class="text-right">
                    <v-btn class="btn-block" :loading="signInSubmitting" :disabled="signInSubmitting" color="primary" @click="_SignIn">Login</v-btn>
                </div>
            </div>
            <div class="col-sm-12 col-md-6 pb-3 pl-md-5">
                <h2>Create Account</h2>
                <!--login-->
                <label class="font-weight-bold">Login Name</label><br>
                <input name="LoginName" class="form-control" v-model="createAlias" /><br>
                <div v-show="!!!createValidation['LoginName.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                <!--email-->
                <label class="font-weight-bold">Email</label><br>
                <input name="Email" type="email" class="form-control" v-model="email" /><br>
                <div v-show="!!!createValidation['Email.Required']" class="text-danger">A valid email is required</div>
                <!--password-->
                <label class="font-weight-bold">Password</label><br>
                <input name="Password" class="form-control" v-model="createPassword" /><br>
                <div v-show="!!!createValidation['Password.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
                <!--submit--> 
                <div class="text-right">
                    <v-btn class="btn-block" :loading="createAccountSubmitting" :disabled="createAccountSubmitting" color="primary" @click="_CreateAccount">Create Account</v-btn>
                </div>
            </div>
        </div>
    </div>

</template>

<script>
    export default {
        props: ['text', 'Alias', "Password", 'baseurl'],
        data: function () {
            return {
                validation: {
                    "LoginName.Required": true,
                    "Password.Required": true
                },
                createValidation: {
                    "LoginName.Required": true,
                    "Password.Required": true,
                    "Email.Required": true
                },
                localAlias: this.Alias,
                localPassword: this.Password,
                createAlias: '',
                createPassword: '',
                email : '',
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
                    "Email.Required": !!this.email && this.email.length > 4,
                };

                let _validation = this.createValidation;
                let valid = Object.keys(_validation).every(function (key) {
                    return _validation[key];
                });

                if (!valid) {
                    this.createAccountSubmitting = false;
                    return;
                }

                this.CreateAccount(this.createAlias,this.email, this.createPassword)
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
    }
</script>
