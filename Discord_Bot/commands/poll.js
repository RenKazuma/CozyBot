const { SlashCommandBuilder } = require("discord.js");
const { ButtonStyle, EmbedBuilder, ButtonBuilder, ActionRowBuilder } = require("discord.js");
const axios = require("axios");

module.exports = {
  data: new SlashCommandBuilder()
    .setName("poll")
    .setDescription("Make poll")
    .addStringOption(option =>
      option.setName("title").setDescription("Set the title").setRequired(true)
    )
    .addStringOption(option =>
      option.setName("button1").setDescription("Set the label for the green button").setRequired(true)
    )
    .addStringOption(option =>
      option.setName("button2").setDescription("Set the label for the red button").setRequired(true)
    )
    .addStringOption(option =>
      option.setName("button3").setDescription("Set the label for the blue button").setRequired(false)
    )
    .addStringOption(option =>
        option.setName("description").setDescription("Set the description for the poll").setRequired(false)
    )
    .addStringOption(option =>
      option.setName("color").setDescription("Set the color").setRequired(false)
    )
    .addBooleanOption(option =>
      option.setName("multiple_choice").setDescription("If user can vote for multiple choices, default is no").setRequired(false)
    ),

  async execute(interaction) {
    const title = interaction.options.getString("title");
    const color = interaction.options.getString("color");
    const btnLabel = [];

    for (let i = 1; i <= 3; i++) {
      const label = interaction.options.getString(`button${i}`);
      if (label) {
        btnLabel.push(label);
      }
    }

    const multipleChoice = interaction.options.getBoolean("multiple_choice") ?? false;
    const footerText = multipleChoice ? process.env.MultipleChoiceTrue : process.env.MultipleChoiceFalse;

    const errorMessages = [];

    const embed = new EmbedBuilder()
      .setTitle(title)
      .setColor("#fb6f92")
      .setThumbnail("https://i.imgur.com/RnATZDl.png")
      .setTimestamp()
      .setDescription(interaction.options.getString("description"))
      .setFooter({ text: `${footerText}\nCreated by ${interaction.user.tag}` });

    if (color && color.match(/^#[0-9A-F]{6}$/i)) {
      const convertedColor = parseInt(color.replace("#", ""), 16);
      embed.setColor(convertedColor);
    } else if (color) {
      errorMessages.push(
        `Your color must be a hexadecimal color code such as #fb6f92. For more help, look here: https://coolors.co/palettes/trending`
      );
    }

    for (let index = 0; index < btnLabel.length; index++) {
        embed.addFields([
          {
            name: btnLabel[index],
            value: "-",
            inline: true,
          },
        ]);
      }

      
    const buttons = [
      new ButtonBuilder().setCustomId("confirm").setLabel(btnLabel[0]).setStyle(ButtonStyle.Success),
      new ButtonBuilder().setCustomId("cancel").setLabel(btnLabel[1]).setStyle(ButtonStyle.Danger),
    ];

    if (btnLabel[2]) {
      buttons.push(new ButtonBuilder().setCustomId("pending").setLabel(btnLabel[2]).setStyle(ButtonStyle.Primary));
    }

    const row = new ActionRowBuilder().addComponents(...buttons);

    await interaction.reply({ embeds: [embed], components: [row] });

    for (const errorMessage of errorMessages) {
       await interaction.followUp({ content: errorMessage, ephemeral: true })
    }
  }
};

console.log("🩰 Poll Command initialized!");
