const { SlashCommandBuilder } = require("discord.js");
const {
  ActionRowBuilder,
  ButtonBuilder,
  ButtonStyle,
  EmbedBuilder,
} = require("discord.js");
const axios = require("axios");

(module.exports = {
  data: new SlashCommandBuilder()
    .setName("poll")
    .setDescription("Make poll")
    .addStringOption((option) =>
      option.setName("title").setDescription("Set the title").setRequired(true)
    )
    .addStringOption((option) =>
      option
        .setName("button_1_label")
        .setDescription("Set the Button 1 Label")
        .setRequired(true)
    )
    .addStringOption((option) =>
      option
        .setName("button_2_label")
        .setDescription("Set the Button 2 Label")
        .setRequired(true)
    )
    .addStringOption((option) =>
      option
        .setName("button_3_label")
        .setDescription("Set the Button 3 Label")
        .setRequired(false)
    )
    .addStringOption((option) =>
      option.setName("color").setDescription("Set the color").setRequired(false)
    )
    .addBooleanOption((option) =>
      option
        .setName("multiple_choice")
        .setDescription("If user can vote for multiple choices, default is no")
        .setRequired(false)
    ),

  async execute(interaction) {
    const title = interaction.options.getString("title");
    const color = interaction.options.getString("color");
    const btnLabel = [];

    if (interaction.options.getString("button_1_label") != null) {
      btnLabel.push(interaction.options.getString("button_1_label"));
    }

    if (interaction.options.getString("button_2_label") != null) {
      btnLabel.push(interaction.options.getString("button_2_label"));
    }

    if (interaction.options.getString("button_3_label") != null) {
      btnLabel.push(interaction.options.getString("button_3_label"));
    }

    const multipleChoice =
      interaction.options.getBoolean("multiple_choice") ?? false;
    var footerText;

    if (multipleChoice) footerText = process.env.MultipleChoiceTrue;
    else footerText = process.env.MultipleChoiceFalse;

    const errorMessages = [];

    var embed = new EmbedBuilder()
      .setTitle(title)
      .setColor("#fb6f92")
      .setThumbnail("https://i.imgur.com/RnATZDl.png")
      .setTimestamp()
      .setFooter({ text: footerText });

    if (color) {
      if (!color.match(/^#[0-9A-F]{6}$/i)) {
        errorMessages.push(
          `Your color must be a hexadecimal color code such as #fb6f92. For more help, look here: https://coolors.co/palettes/trending`
        );
      } else {
        let convertedColor = parseInt(color.replace("#", ""), 16);
        embed.setColor(convertedColor);
      }
    }

    for (let index = 0; index < btnLabel.length; index++) {
console.log(`btn${index + 1}`)
      embed.addFields([
        {
          name: `btn${index + 1}`,
          value: "-",
          inline: true,
        },
      ]);
    }
    const confirm = new ButtonBuilder()
      .setCustomId("confirm")
      .setLabel(btnLabel[0])
      .setStyle(ButtonStyle.Success);

    const cancel = new ButtonBuilder()
      .setCustomId("cancel")
      .setLabel(btnLabel[1])
      .setStyle(ButtonStyle.Danger);

      var pending;

    if (btnLabel[2]) {
       pending = new ButtonBuilder()
        .setCustomId("pending")
        .setLabel(btnLabel[2])
        .setStyle(ButtonStyle.Primary);
    }

    var row;
    if(pending){
        row = new ActionRowBuilder().addComponents(confirm, cancel, pending);
    } else {
        row = new ActionRowBuilder().addComponents(confirm, cancel);
    }


    // Send the main reply with the embed (visible to everyone)
    await interaction.reply({ embeds: [embed], components: [row] });

    // Send error messages as ephemeral replies (visible only to the user who used the command)
    for (const errorMessage of errorMessages) {
      console.log(errorMessage + "hhh");
      await interaction.followUp({ content: errorMessage, ephemeral: true });
    }
  },
}),
  console.log("🩰 Poll Command initialisiert!");
