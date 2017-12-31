<template>
    <div>
        <h2>Translations</h2>
        <p>Translate your all keys here and it will be stored directly, it autotranslation is enabled you will get suggestions translated from main language</p>
        <span>{{nokeys}} keys</span>
        <p v-if="!translations"><em><i class="fas fa-spinner fa-spin" />&nbsp;Loading</em></p>
        <ul class="langs">
            <li v-for="l in languages">
                {{l.name}}
            </li>
        </ul>
        <table class="table" v-if="translations">
            <thead>
                <tr>
                    <th class="key">Key</th>
                    <th>Values</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(value,key) in translations.values">
                    <td class="key">
                        <div class="keyvalue">{{ key }}</div>
                        <div class="tools">
                            <span @click="dodelete(key)"><i class="fa fa-times" />Delete</span>
                        </div>
                    </td>
                    <td class="valuetd">
                        <div class="value" v-for="(lang,idx) in value"><transinput :locale="translations.locales[idx]" :transkey="key" :value="lang" /></div>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
</template>
<script>
import Vue from 'vue'
import TransInput from './transinput'

Vue.component('transinput', TransInput);

export default {
    data() {
        return {
            translations: {}
        }
    },

    methods: {
        reload:function() {
            this.$http
                .get('/api/translation')
                .then(response => {
                    this.translations = response.data;
                });
        },
        dodelete:function(key) {
            this.$http.delete('/api/translation/'+key).then(response => {
                delete this.translations.values[key];
                this.reload();
            });
        }
    },
    computed: {
        nokeys: function() {
            if (this.translations.values)
                return Object.keys(this.translations.values).length;
            return 0;
        },
        languages: function(l) {
            let languageNames = {
                'se': 'Swedish',
                'sv': 'Swedish',
                'no': 'Norwegian',
                'en': 'English',
                'dk': 'Danish'
            };
            if (this.translations.locales)
                return this.translations.locales.map(function(v,i) {
                    return {
                        locale: v,
                        idx: i,
                        name: languageNames[v]
                    };
                });
        
            return [];
        }
    },
    async created() {
        try {
            let response = await this.$http.get('/api/translation')
            console.log(response.data);
            this.translations = response.data;
        } catch (error) {
            console.log(error)
        }
    }
}
</script>
<style>
    .valuetd {
        background:#eee;
    }

    .langs {
        list-style:none;
        margin:0;
        padding:0;
    }

    .keyvalue {
        text-align:right;
    }
    .tools {
        text-align:right;
    }
    .tools > span {
        margin-left: 10px;
        padding: 4px 8px;
        font-size: 12px;
        color: #fff;
        border-radius: 20px;
        background-color: red;
    }

    .langs li {
        display:inline-block;
        margin-right: 10px;
        padding: 4px 8px;
        color: #fff;
        border-radius: 20px;
        background-color: green;
    }

    table.table {
        width: auto;
        min-width: 40%;
    }

    .value input {
        width: 190px;
        display:inline-block;
    }
</style>