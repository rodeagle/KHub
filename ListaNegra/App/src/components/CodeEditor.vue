<template>
    <prism-editor :code="code" language="js"></prism-editor>
</template>

<script>
    import PrismEditor from 'vue-prism-editor';
    export default {
        components: {
            PrismEditor
        }
    }
</script>