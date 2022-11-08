import { basename } from 'path'

export function process(_sourceText, sourcePath) {
    return {
        code: `module.exports = ${JSON.stringify(basename(sourcePath))};`,
    }
}