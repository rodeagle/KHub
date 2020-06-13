<template>

    <div class="p-3">
        <h2>Add Current contribution to a Project</h2>
        <label class="font-weight-bold">Projects</label><br>
        <select v-model="selected" class="form-control" placeholder="Select a Project">
            <option value="">-- Select a Project --</option>
            <option v-for="option in options" v-bind:value="option.value">
                {{ option.text }}
            </option>
        </select>
        <div v-show="!validation['Project.Required']" class="text-danger">You must select a project first</div>
        <br />
        <div class="text-right">
            <v-btn class="btn-block" :loading="favoriteSubmit" :disabled="favoriteSubmit" color="primary" @click="_AddToProject">Add</v-btn>
        </div>
    </div>
      
</template>

<script>

    export default {
        props: ['PostID', 'Projects'],
        data: function () {
            return {
                // create project
                validation: {
                    "Project.Required": true
                },
                selected: '',
                options: [],
                favoriteSubmit : false
            };
        },
        mounted: function () {
            let options = this.Projects;
            let newOpt = options.map((x, i) => {
                return {
                    text: x.Name,
                    value: x.ProjectID
                };
            });
            this.options = [...this.options, ...newOpt];
        },
        methods: {
            _AddToProject: function () {

                this.validation = {
                    "ProjectName.Required": this.selected > 0,
                };

                let valid = this.Validate(this.validation);

                if (!valid) {
                    return;
                }

                this.favoriteSubmit = true;

                this.AddPostToProject(this.PostID, this.selected)
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
                        this.favoriteSubmit = false;
                    });

            }
        }
    }

</script>