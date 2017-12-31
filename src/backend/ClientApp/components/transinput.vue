<template>
    <div class="ff">
        <span class="langname">{{langName}}</span>
        <span class="oldvalue" v-if="!editing" v-on:click="editing=!editing">{{oldValue}}</span>
        <div v-if="editing">
            <span v-if="saving" class="saving">Saving</span>
            <textarea v-if="!saving" v-model="newValue" @blur="save" @focus="getsugg" /> <span v-if="oldValue!=newValue">({{oldValue}})</span>
            <span @click="getsugg">{{suggest}}</span>
            <span v-on:click="editing=false"><i class="fa fa-times" /></span>
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
            suggest: 'Loading...',
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
            let languageNames = {
                'se': 'Swedish',
                'sv': 'Swedish',
                'no': 'Norwegian',
                'en': 'English',
                'dk': 'Danish'
            };
            if (languageNames[this.lang]) {
                return languageNames[this.lang]+':';
            }
            return this.lang;
        }
    }
}
</script>
<style>
    .ff input {
        border:0;
        border-bottom: dotted 1px green;
        background:transparent;
    }

    .langname {
        color:#333;
    }

    .oldvalue {
        border-bottom: dotted 1px blue;
        padding: 2px 6px;
        display: inline-block;
    }

    span.saving {

    }
</style>