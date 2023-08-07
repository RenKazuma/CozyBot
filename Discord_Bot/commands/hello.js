const { SlashCommandBuilder } = require('discord.js');

module.exports = {
    data: new SlashCommandBuilder()
      .setName('hello')
      .setDescription('Say hello')
      .addUserOption((user) =>
      user
        .setName("user")
        .setDescription("Say hello to someone else!")
        .setRequired(false)
    ),

    async execute(interaction) {

       //Variables
    const user = interaction.options.get("user")?.value ?? interaction.user.id;

      await interaction.reply(`Hello <@${user}>`);
    },
  };

console.log("⚾ Hello Command initialisiert!");