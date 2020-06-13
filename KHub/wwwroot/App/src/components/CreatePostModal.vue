<template>
    <!--// to add later on template vuetags
        /*<label class="font-weight-bold">Tags</label>*/
        /*<small>What defines the solution (multiple answers)</small>*/
        /*<vue-tag-input v-model="Tags"></vue-tag-input>*/-->
      
    <div class="p-3">
        <h2>Create New Post</h2>
        <label class="font-weight-bold">Title</label>
        <small>Make it sound and clear</small><br>
        <input class="form-control" v-model="title" />
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
        <vue-editor v-model="description" :editorToolbar="customToolbar"></vue-editor>
        <!--<textarea rows="10" class="w-100 form-control" v-model="description"></textarea><br>-->
        <div v-show="!!!createPostValidation['Description.Required']" class="text-danger">This field is required and with a min of 4 letters</div>
        <label class="font-weight-bold">Code</label>
        <small>Code section to attach</small>
        <v-btn color="primary" @click="AddCodeSection">Add a code section</v-btn>
        <prism-editor class="pb-2" v-for="(code, index) in codes"  v:key="codes[index]" :code="codes[index]" v-model="codes[index]" language="js" :line-numbers="true"></prism-editor>
        <br>
        <div class="row">
            <div class="col-12 col-sm-12 col-lg-6">
                <v-btn class="btn-block" color="secondary" @click="_CloseModal">Cancel</v-btn>
            </div>
            <div class="col-12 col-sm-12 col-lg-6">
                <v-btn class="btn-block" :loading="createProjectSubmitting" :disabled="createProjectSubmitting" color="primary" @click="_CreatePost">Create</v-btn>
            </div>
        </div>
    </div>
</template>


<script>
    import vuetag from '@johmun/vue-tags-input';
    import VuePrismEditor from "vue-prism-editor";
    import { VueEditor } from "vue2-editor";

    export default {
        components: {
            'vue-tag-input': vuetag,
            'prism-editor': VuePrismEditor,
            VueEditor
        },
        props: {
        },
        data: function () {
            return {
                customToolbar: [
                    [{ 'font': [] }],
                    [{ 'header': [false, 1, 2, 3, 4, 5, 6, ] }],
                    [{ 'size': ['small', false, 'large', 'huge'] }],
                    ['bold', 'italic', 'underline', 'strike'],
                    [{'align': ''}, {'align': 'center'}, {'align': 'right'}, {'align': 'justify'}],
                    [{ 'header': 1 }, { 'header': 2 }],
                    ['blockquote', 'code-block'],
                    [{ 'list': 'ordered'}, { 'list': 'bullet' }, { 'list': 'check' }],
                    [{ 'script': 'sub'}, { 'script': 'super' }],
                    [{ 'indent': '-1'}, { 'indent': '+1' }],
                    [{ 'color': [] }, { 'background': [] }],
                    ['link', 'formula'],
                    [{ 'direction': 'rtl' }],
                    ['clean'],
                ],
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
                codes: [],
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
            AddCodeSection: function () {
                this.codes.push('');
            },
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
                        setTimeout(() => {
                            location.reload();
                        }, 1000);
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
