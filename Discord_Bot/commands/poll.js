const { SlashCommandBuilder } = require('discord.js');
const {  ActionRowBuilder, ButtonBuilder, ButtonStyle, EmbedBuilder } = require('discord.js');

module.exports = {
    data: new SlashCommandBuilder()
      .setName('poll')
      .setDescription('Make poll')
        .addStringOption(option  => 
            option 
            .setName('title')
            .setDescription('Set the title')
            .setRequired(true))
        .addStringOption(option  => 
            option 
            .setName('button_1_label')
            .setDescription('Set the Button 1 Label')
            .setRequired(true))
        .addStringOption(option  => 
            option 
            .setName('button_2_label')
            .setDescription('Set the Button 1 Label')
            .setRequired(true))
        .addStringOption(option  => 
            option 
            .setName('button_3_label')
            .setDescription('Set the Button 1 Label')
            .setRequired(true))
        .addStringOption(option  => 
            option 
            .setName('color')
            .setDescription('Set the color')
            .setRequired(false)
            .addChoices(
				{ name: 'Purple', value: '#5e548e' },
				{ name: 'Blue', value: '#90e0ef' },
				{ name: 'Green', value: '#a7c957' },
			)),

    async execute(interaction) {

        const title = interaction.options.getString('title');
        const color = interaction.options.getString('color');
        const btn1 = interaction.options.getString('button_1_label');
        const btn2 = interaction.options.getString('button_2_label');
        const btn3 = interaction.options.getString('button_3_label');

         const errorMessages = [];

        var embed = new EmbedBuilder()
          .setTitle(title)
          .setColor('#fb6f92')
          .setThumbnail('https://i.imgur.com/RnATZDl.png')
          .setTimestamp();
    
        if (color) {
          if (!color.match(/^#[0-9A-F]{6}$/i)) {
            errorMessages.push(`Your color must be a hexadecimal color code such as #fb6f92. For more help, look here: https://coolors.co/palettes/trending`);
          } else {
            let convertedColor = parseInt(color.replace("#", ""), 16);
            embed.setColor(convertedColor);
          }
        }

        embed.addFields([  
            {   
                name: btn1,    
                value: '-',
                inline:  true
            },
            {   
                name: btn2,    
                value: '-',
                inline:  true
            },
            {   
                name: btn3,    
                value: '-',
                inline:  true
            }
        ]);
    
    	const confirm = new ButtonBuilder()
			.setCustomId('confirm')
			.setLabel(btn1)
			.setStyle(ButtonStyle.Success);

		const cancel = new ButtonBuilder()
			.setCustomId('cancel')
			.setLabel(btn2)
			.setStyle(ButtonStyle.Danger);

            const pending = new ButtonBuilder()
			.setCustomId('pending')
			.setLabel(btn3)
			.setStyle(ButtonStyle.Primary);


            const row = new ActionRowBuilder()
			.addComponents(confirm, pending, cancel);

        // Send the main reply with the embed (visible to everyone)
        await interaction.reply({ embeds: [embed], components: [row]  });
    
        // Send error messages as ephemeral replies (visible only to the user who used the command)
        for (const errorMessage of errorMessages) {
          await interaction.followUp({ content: errorMessage, ephemeral: true });
        }
    
    },
},
  

console.log("🩰 Poll Command initialisiert!");