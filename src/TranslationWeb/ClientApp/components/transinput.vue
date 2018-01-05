<template>
    <div class="ff">
        <span class="langname" v-if="!isNew">{{langName}}</span>
        <span class="oldvalue" v-if="!editing" v-on:click="editing=!editing">{{oldValue}}</span>
        <div v-if="editing" class="editbox">
            <span v-if="saving" class="saving">Saving</span>
            <div v-if="isNew">
                <label>Key</label>
                <input v-model="key" placeholder="New key" />
                <label>Default language translation</label>
            </div>
            <textarea v-if="!saving" v-model="newValue" @focus="getsugg" /> <span v-if="oldValue!=newValue">({{oldValue}})</span>
            <div @click="getsugg" v-if="hasTranslated">{{suggest}}</div>
            <div class="tools lefttools">
                <span @click="save" class="savebtn"><i class="fa fa-check" /></span>
                <span v-if="!isNew" @click="getsugg"><i class="fa fa-magic" /></span>
                <span v-on:click="editing=false" class="closebtn" v-if="!isNew"><i class="fa fa-times" /></span>
            </div>
        </div>
    </div>
</template>
<script>

export default {
    props: ['transkey','value','locale','isnew','isopen'],
    data() {
        return {
            saving: false,
            editing: !!this.isopen,
            isNew: this.isnew,
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

    div.lefttools {
        text-align:left;
    }

    .editbox {
        position:relative;
        background-color: #eee;
        border:solid 1px #ccc;
        padding:10px;
    }

    .closebtn {

    }

    textarea {
        width:300px;
        border:solid 1px #ccc;
    }

    .langname {
        color:#333;
    }

    .tools > .savebtn {
        background-color: #27ae60;
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