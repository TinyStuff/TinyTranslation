<template>
    <div class="ff">
        <span class="langname">{{langName}}</span>
        <span class="oldvalue" v-if="!editing" v-on:click="editing=!editing">{{oldValue}}</span>
        <div v-if="editing" class="editbox">
            <span v-if="saving" class="saving">Saving</span>
            <textarea v-if="!saving" v-model="newValue" @blur="save" @focus="getsugg" /> <span v-if="oldValue!=newValue">({{oldValue}})</span>
            <div @click="getsugg">{{suggest}}</div>
            <div class="tools lefttools">
                <span @click="save" class="savebtn"><i class="fa fa-check" /></span>
                <span v-on:click="editing=false" class="closebtn"><i class="fa fa-times" /></span>
            </div>
        </div>
    </div>
</template>
<script>

export default {
    props: ['transkey','value','locale'],
    data() {
        return {
            saving: false,
            editing: false,
            lang: this.locale,
            key: this.transkey,
            hasTranslated: false,
            suggest: 'Click for autotranslation...',
            oldValue: this.value,
            newValue: this.value
        }
    },

    methods: {
        getsugg:function() {
            if (!this.hasTranslated) {
                this.$http.get('/api/admin/'+this.key+'/'+this.lang).then(resp=> {
                    this.suggest = resp.data;
                    this.hasTranslated = true;
                });
            }
            else {
                this.newValue = this.suggest;
            }
        },
        save:function() {
            if (this.newValue != this.oldValue) {
                this.saving = true;
                this.$http.put('/api/translation/'+this.lang+'/'+this.key+'/'+this.newValue)
                    .then(response => {
                        this.saving = false;
                        this.oldValue = this.newValue;
                        this.editing = false;
                    });
            }
        }
    },
    computed: {
        langName: function() {

            if (window.$languageNames[this.lang]) {
                return window.$languageNames[this.lang]+':';
            }
            return this.lang;
        }
    }
}
</script>
<style>
    .ff input {
        border:0;
        border-bottom: dotted 1px #53B7CE;
        background:transparent;
    }
    .lefttools {
        text-align:left;
    }

    .editbox {
        position:relative;
        background-color: #ddd;
        border:solid 1px #ccc;
        padding:5px;
    }

    .closebtn {

    }

    .langname {
        color:#333;
    }

    .tools > .savebtn {
        background-color: green;
    }

    .tools > .closebtn {
        background-color: #333;
    }

    .oldvalue {
        border-bottom: dotted 1px #53B7CE;
        padding: 2px 6px;
        display: inline-block;
    }

    span.saving {

    }
</style>