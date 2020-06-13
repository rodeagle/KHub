<template>

    <div class="p-3">
        <h2>Create Project</h2>
        <label class="font-weight-bold">Project Name</label><br>
        <input name="ProjectName" class="form-control" v-model="projectName" /><br>
        <div v-show="!!!createProjectValidation['ProjectName.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
        <v-switch flat dense v-model="projectIsPublic" color="success" inset :label="'Make the project public'"></v-switch>
        <div class="text-right">
            <v-btn class="btn-block" :loading="createProjectSubmitting" :disabled="createProjectSubmitting" color="primary" @click="_CreateProject">Create</v-btn>
        </div>
    </div>

</template>

<script>

    export default {

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

    }


</script>

